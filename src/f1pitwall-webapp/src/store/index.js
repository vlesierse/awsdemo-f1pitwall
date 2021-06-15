import { configureStore } from '@reduxjs/toolkit'
import reduxWebsocket from '@giantmachines/redux-websocket';
import { routerMiddleware } from 'connected-react-router'
import { createBrowserHistory } from 'history'

import rootReducer from './reducers'

export const history = createBrowserHistory();

const reduxWebsocketMiddleware = reduxWebsocket();

export default function configureAppStore(preloadedState) {
  const store = configureStore({
    reducer: rootReducer(history),
    middleware: (getDefaultMiddleware) =>
      getDefaultMiddleware().concat(routerMiddleware(history)).concat(reduxWebsocketMiddleware),
    preloadedState,
  })

  if (process.env.NODE_ENV !== 'production' && module.hot) {
    module.hot.accept('./reducers', () => store.replaceReducer(rootReducer))
  } 

  return store
}