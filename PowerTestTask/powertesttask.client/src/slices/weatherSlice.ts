import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { type Current, type Days, type Hours } from '../models';

export const weatherSlice = createApi({
    reducerPath: 'weather',
    baseQuery: fetchBaseQuery({
        baseUrl: 'http://localhost:5014/WeatherForecast'
    }),
    endpoints(builder) {
        return {
            fetchCurrent: builder.query<Current, void>({
                query() {
                    return "/current"
                }
            }),
            fetchHours: builder.query<Hours, void>({
                query() {
                    return "/forecast/hours"
                }
            }),
            fetchDays: builder.query<Days, void>({
                query() {
                    return "/forecast/days"
                }
            })
        }
    }
})

export const { useFetchCurrentQuery, useFetchHoursQuery, useFetchDaysQuery } = weatherSlice;