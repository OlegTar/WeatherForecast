import type { FC } from "react"
import type { Hour } from "./models/hours"

export const HoursTable: FC<{ hours: Hour[] }> = ({ hours }) => {
    return <table className="table">
        <thead>
            <tr>
                <th scope="col">Час</th>
                <th scope="col">Температура</th>
            </tr>
        </thead>
        <tbody>
            {hours.map(hour =>
                <tr key={hour.hour}>
                    <td>{hour.hour}</td>
                    <td>{hour.temperature}</td>
                </tr>
            )}
        </tbody>
    </table>
}