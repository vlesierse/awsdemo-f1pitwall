import { connect, disconnect, send } from '@giantmachines/redux-websocket';

export const connectSession = () => {
    return (dispatch) => {
        dispatch(connect(`wss://fexjfyvwt3.execute-api.eu-west-1.amazonaws.com/production`));
    }
}

export const joinSession = (session) => {
    return (dispatch) => {
        dispatch(send({ action: 'JOIN', payload: { session } }));
    }
}

export const leaveSession = () => {
    return (dispatch) => {
        dispatch(disconnect());
    }
}