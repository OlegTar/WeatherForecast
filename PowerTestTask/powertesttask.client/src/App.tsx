import './App.css';
import { useFetchCurrentQuery, useFetchHoursQuery, useFetchDaysQuery } from './slices/weatherSlice'

function App() {
    const { isError: currentIsError, currentIsFetching, currentIsSuccess, currentData: current } = useFetchCurrentQuery();
    const { isError: hoursIsError, hoursIsFetching, hoursIsSuccess, currentData: hours } = useFetchHoursQuery();
    const { isError: daysIsError, daysIsFetching, daysIsSuccess, currentData: days } = useFetchDaysQuery();
    
    return (
        <div className="container text-center">
            <div className="row">
                <div className="col">
                    Column
                </div>
                <div className="col">
                    Column
                </div>
                <div className="col">
                    Column
                </div>
            </div>
        </div>
    );
}

export default App;