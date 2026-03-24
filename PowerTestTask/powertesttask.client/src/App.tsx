import './App.css';
import { useFetchCurrentQuery } from './slices/weatherSlice';

function App() {
    const { isError: currentIsError, isFetching: currentIsFetching, isSuccess: currentIsSuccess, currentData: current } = useFetchCurrentQuery();
    //const { isError: hoursIsError, hoursIsFetching, hoursIsSuccess, currentData: hours } = useFetchHoursQuery();
    //const { isError: daysIsError, daysIsFetching, daysIsSuccess, currentData: days } = useFetchDaysQuery();

    return (
        <div className="container text-center">
            <div className="row">
                <div className="col">
                    <div className='mt-6'>Текущая дата:</div>
                    <button type="button" className="btn btn-primary">Обновить</button>
                    {currentIsFetching &&
                        <div className="spinner-border" role="status">
                            <span className="sr-only"></span>
                        </div>
                    }
                    {currentIsSuccess && <div>{current!.temperatureC}</div>}
                    {currentIsError && <div>произошла ошибка</div>}
                </div>
                <div className="col">
                    Почасовая
                </div>
                <div className="col">
                    Дневная
                </div>
            </div>
        </div>
    );
}

export default App;