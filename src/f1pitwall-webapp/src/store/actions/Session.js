import { connect, disconnect, send } from '@giantmachines/redux-websocket';
import config from '../../AppConfig';

export const connectSession = () => {
    return (dispatch) => {
        dispatch(connect(config.websocketUrl));
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