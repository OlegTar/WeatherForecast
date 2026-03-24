import { configureStore } from '@reduxjs/toolkit'
import { weatherSlice } from './slices/weatherSlice';

export const store = configureStore({
    reducer: {
        [weatherSlice.reducerPath]: weatherSlice.reducer
    },
    middleware: (getDefaultMiddleware) => {
        return getDefaultMiddleware().concat(weatherSlice.middleware);
    }
});

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;