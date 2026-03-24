import './App.css';
import { HoursTable } from './HoursTable';
import { useFetchCurrentQuery, useFetchHoursQuery, useFetchDaysQuery } from './slices/weatherSlice';

function App() {
    const { isError: currentIsError, isFetching: currentIsFetching, isSuccess: currentIsSuccess, currentData: current, refetch: refetchCurrent } = useFetchCurrentQuery();
    const { isError: hoursIsError, isFetching: hoursIsFetching, isSuccess: hoursIsSuccess, currentData: hours, refetch: refetchHours } = useFetchHoursQuery();
    const { isError: daysIsError, isFetching: daysIsFetching, isSuccess: daysIsSuccess, currentData: days, refetch: refetchDays } = useFetchDaysQuery();

    return (
        <div className="container text-center">
            <div className="row">
                <div className="col">
                    <div className='mt-3'>Текущая дата:</div>
                    <button type="button" className="btn btn-primary" onClick={refetchCurrent}>Обновить</button>
                    {currentIsFetching &&
                        <div className="mt-1">
                            <div className="spinner-border" role="status">
                                <span className="sr-only"></span>
                            </div>
                        </div>
                    }
                    <div className="mt-3">
                        {currentIsSuccess && !currentIsFetching && current!.temperatureC}
                        {currentIsError && <>Произошла ошибка</>}
                    </div>
                </div>
                <div className="col">
                    <div className='mt-3'>Почасовая:</div>
                    <button type="button" className="btn btn-primary" onClick={refetchHours}>Обновить</button>
                    {hoursIsFetching &&
                        <div className="mt-1">
                            <div className="spinner-border" role="status">
                                <span className="sr-only"></span>
                            </div>
                        </div>
                    }
                    <div className="mt-3">
                    {hoursIsError && <>Произошла ошибка</>}
                    {hoursIsSuccess && !hoursIsFetching &&
                        <>
                            <ul className="nav nav-tabs" id="myTab" role="tablist">
                                <li className="nav-item" role="presentation">
                                <button className="nav-link active" id="home-tab" data-bs-toggle="tab" data-bs-target="#today-tab-pane" type="button" role="tab" aria-controls="today-tab-pane" aria-selected="true">Сегодня</button>
                                </li>
                                <li className="nav-item" role="presentation">
                                 <button className="nav-link" id="profile-tab" data-bs-toggle="tab" data-bs-target="#tomorrow-tab-pane" type="button" role="tab" aria-controls="tomorrow-tab-pane" aria-selected="false">Завтра</button>
                                </li>
                            </ul>
                            <div className="tab-content hours" id="myTabContent">
                                <div className="tab-pane fade show active" id="today-tab-pane" role="tabpanel" aria-labelledby="home-tab" tabIndex={0}><HoursTable hours={hours.today}></HoursTable></div>
                                <div className="tab-pane fade" id="tomorrow-tab-pane" role="tabpanel" aria-labelledby="profile-tab" tabIndex={0}><HoursTable hours={hours.tomorrow}></HoursTable></div>
                            </div>                            
                        </>
                    }
                    </div>
                </div>
                <div className="col">
                    <div className='mt-3'>Дневная:</div>
                    <button type="button" className="btn btn-primary" onClick={refetchDays}>Обновить</button>
                    {daysIsFetching &&
                        <div className="mt-1">
                            <div className="spinner-border" role="status">
                                <span className="sr-only"></span>
                            </div>
                        </div>
                    }
                    <div className="mt-3">
                        {daysIsError && <>Произошла ошибка</>}
                        {daysIsSuccess && !daysIsFetching &&
                            <table className="table text-start">
                                <tbody>
                                    <tr>
                                        <td>Завтра:</td>
                                        <td>{days!.tomorrowTemperature || "N/A"}</td>
                                    </tr>
                                    <tr>
                                        <td>Послезавтра:</td>
                                        <td>{days!.dayAfterTomorrowTemperature || "N/A"}</td>
                                    </tr>
                                    <tr>
                                        <td>После послезавтра:</td>
                                        <td>{days!.twoDaysAfterTomorrowTemperature || "N/A"}</td>
                                    </tr>
                                </tbody>
                            </table>
                        }
                    </div>
                </div>
            </div>
        </div>
    );
}

export default App;