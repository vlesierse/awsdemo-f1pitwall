import * as s3 from 'aws-cdk-lib/aws-s3';
import * as cdk from 'aws-cdk-lib';
import * as iam from 'aws-cdk-lib/aws-iam';
import * as lambda from 'aws-cdk-lib/aws-lambda';
import * as kinesis from 'aws-cdk-lib/aws-kinesis';
import * as dynamodb from 'aws-cdk-lib/aws-dynamodb';
import * as firehose from 'aws-cdk-lib/aws-kinesisfirehose';
import * as eventsources from 'aws-cdk-lib/aws-lambda-event-sources';
import * as apigateway from '@aws-cdk/aws-apigatewayv2-alpha';
import * as integrations from '@aws-cdk/aws-apigatewayv2-integrations-alpha';
import { DotNetFunction } from '@xaaskit-cdk/aws-lambda-dotnet';

import { Construct } from 'constructs';
import { OpenSearch } from './constructs/opensearch';

export class F1PitwallStack extends cdk.Stack {
  constructor(scope: Construct, id: string, props?: cdk.StackProps) {
    super(scope, id, props);

    // Artifacts Bucket
    var artifactsBucket = new s3.Bucket(this, 'ArtifactsBucket');

    // Telemetry Stream
    const telemetryStream = new kinesis.Stream(this, 'TelemetryStream', {
      streamName: 'F1Pitwall-TelemetryStream',
      streamMode: kinesis.StreamMode.ON_DEMAND
    });

    // Results
    const opensearch = new OpenSearch(this, 'OpenSearch');
    const analyticsStreamRole = new iam.Role(this, 'AnalyticsStreamRole', {
      assumedBy: new iam.ServicePrincipal('firehose.amazonaws.com')
    });
    artifactsBucket.grantWrite(analyticsStreamRole);
    analyticsStreamRole.addToPolicy(
      new iam.PolicyStatement({
        actions: ['es:*'],
        resources: [
          opensearch.domain.domainArn,
          opensearch.domain.domainArn + '/*',
        ],
      })
    );
    analyticsStreamRole.addToPolicy(
      new iam.PolicyStatement({
        effect: iam.Effect.ALLOW,
        resources: [
          "arn:aws:logs:*:*:log-group:/aws/kinesisfirehose/F1Pitwall-AnalyticsStream:*",
        ],
        actions: ["logs:PutLogEvents"],
      })
    );

    analyticsStreamRole.addToPolicy(
      new iam.PolicyStatement({
        effect: iam.Effect.ALLOW,
        resources: [telemetryStream.streamArn],
        actions: [
          'kinesis:DescribeStream',
          'kinesis:GetShardIterator',
          'kinesis:GetRecords'
        ],
      })
    );
    
    const analyticsStream = new firehose.CfnDeliveryStream(this, 'AnalyticsStream', {
      deliveryStreamName: 'F1Pitwall-AnalyticsStream',
      deliveryStreamType: 'KinesisStreamAsSource',
      kinesisStreamSourceConfiguration: {
        kinesisStreamArn: telemetryStream.streamArn,
        roleArn: analyticsStreamRole.roleArn,
      },
      amazonopensearchserviceDestinationConfiguration: {
        domainArn: opensearch.domain.domainArn,
        indexName: 'results',
        s3BackupMode: "AllDocuments",
        indexRotationPeriod: 'OneDay',
        bufferingHints: {
          intervalInSeconds: 60,
          sizeInMBs: 1,
        },
        retryOptions: {
          durationInSeconds: 300,
        },
        roleArn: analyticsStreamRole.roleArn,
        cloudWatchLoggingOptions: {
          enabled: true,
          logGroupName: "/aws/kinesisfirehose/MigrationAssistant-AnalyticsStream",
          logStreamName: "OpenSearchDelivery",
        },
        s3Configuration: {
          bucketArn: artifactsBucket.bucketArn,
          prefix: 'results/',
          bufferingHints: {
            intervalInSeconds: 300,
            sizeInMBs: 5,
          },
          compressionFormat: "GZIP",
          roleArn: analyticsStreamRole.roleArn,
          cloudWatchLoggingOptions: {
            enabled: true,
            logGroupName: "/aws/kinesisfirehose/F1Pitwall-AnalyticsStream",
            logStreamName: "S3Delivery",
          },
        },
      }
    });
    analyticsStream.node.addDependency(analyticsStreamRole);

    // Web Socket API
    const apiSessionsTable = new dynamodb.Table(this, 'WebSocketApiTable', {
      tableName: 'F1Pitwall-WebSocketApiTable',
      sortKey: { name: 'SK', type: dynamodb.AttributeType.STRING },
      billingMode: dynamodb.BillingMode.PAY_PER_REQUEST,
      partitionKey: { name: 'PK', type: dynamodb.AttributeType.STRING },
    });
    // WebSocket Api
    const webSocketApiFunction = new DotNetFunction(this, 'WebSocketApiFunction', {
      projectDir: 'src/F1Pitwall.WebSocketApi',
      environment: {
        'WEBSOCKETAPI_TABLE': apiSessionsTable.tableName,
      }
    });
    apiSessionsTable.grantReadWriteData(webSocketApiFunction);
    const webSocketApi = new apigateway.WebSocketApi(this, 'WebSocketApi', {
      apiName: 'F1Pitwall-WebSocketApi',
      defaultRouteOptions: { integration: new integrations.WebSocketLambdaIntegration('DefaultIntegration', webSocketApiFunction) },
    });
    const webSocketApiStageProduction = new apigateway.WebSocketStage(this, 'WebSocketApiStageProduction', {
      webSocketApi,
      stageName: 'production',
      autoDeploy: true,
    });
    new cdk.CfnOutput(this, 'WebSocketApiUrl', { value: webSocketApiStageProduction.url });

    // Processor
    const processorFunction = new DotNetFunction(this, 'ProcessorFunction', {
      projectDir: 'src/F1Pitwall.Processor',
      environment: {
        'WEBSOCKETAPI_TABLE': apiSessionsTable.tableName,
        'WEBSOCKETAPI_URL': webSocketApiStageProduction.callbackUrl,
      }
    });
    processorFunction.addEventSource(new eventsources.KinesisEventSource(telemetryStream, { startingPosition: lambda.StartingPosition.LATEST, batchSize: 10 }));
    webSocketApi.grantManageConnections(processorFunction);
    apiSessionsTable.grantReadData(processorFunction);
  }
}
