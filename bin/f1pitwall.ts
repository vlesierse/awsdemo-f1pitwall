#!/usr/bin/env node
import 'source-map-support/register';
import * as cdk from 'aws-cdk-lib';
import { F1PitwallStack } from '../lib/f1pitwall-stack';

const app = new cdk.App();
new F1PitwallStack(app, 'F1Pitwall', {});