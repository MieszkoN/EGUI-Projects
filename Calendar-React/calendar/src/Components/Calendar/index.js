import React,{Component} from 'react';
import moment from 'moment';
import { Link } from 'react-router-dom/cjs/react-router-dom.min';
import './calendar.css';

export default class Calendar extends Component {
    constructor(props) {
        super(props);
        this.width = props.width || "350px";
        this.style = props.style || {};
    }


    state = {
        dateContext: moment(),
        today: moment(),
        showMonthPopup: false,
        showYearPopup: false,
    }

    months = moment.months();

    year = () => {
        return this.state.dateContext.format("Y");
    }

    month = () => {
        return this.state.dateContext.format("MMMM");
    }

    daysInMonth = () => {
        return this.state.dateContext.daysInMonth();
    }

    currentDate = () => {
        return this.state.dateContext.get("date");
    }

    currentDay = () => {
        return this.state.dateContext.format("D");
    }

    firstDayOfMonth = () => {
        let dateContext = this.state.dateContext;
        let firstDay = moment(dateContext).startOf('month').format('d');
        return firstDay;
    }

    setMonth = (month) => {
        let monthNo = this.months.indexOf(month);
        let dateContext = Object.assign({}, this.state.dateContext);
        dateContext = moment(dateContext).set("month", monthNo);
        this.setState({
            dateContext: dateContext
        });
    }

    nextMonth = () => {
        let dateContext = Object.assign({}, this.state.dateContext);
        dateContext = moment(dateContext).add(1, "month");
        this.setState({
            dateContext: dateContext
        });
        this.props.onNextMonth && this.props.onNextMonth();
    }

    prevMonth = () => {
        let dateContext = Object.assign({}, this.state.dateContext);
        dateContext = moment(dateContext).subtract(1, "month");
        this.setState({
            dateContext: dateContext
        });
        this.props.onPrevMonth && this.props.onPrevMonth();
    }
    
    render() {

        let daysInPrevMonth = [];
        for (let i = 0; i < this.firstDayOfMonth(); i++) {
                var m = moment().month(this.month()).format("M");
                var y;
                if(m===1) {
                    y = this.year()-1;
                    m=12;
                } else {
                    y = this.year();
                    m=m-1;
                }
                var d = new Date(y, m, 0).getDate();
                daysInPrevMonth.push(<td className = "daysOtherMonth" key = {d-this.firstDayOfMonth()+i+1+"-"+this.month()+"-"+this.year()+"prevmonth"}>
                    <span>{d-this.firstDayOfMonth()+i+1}</span>
                </td>
            );
        }

        let daysInMonth = [];
        for (let d = 1; d <= this.daysInMonth(); d++) {
            daysInMonth.push(
                <td key={d+"-"+this.month()+"-"+this.year()} className="day">
                   <span><Link className="dayLink" to={{ pathname: '/singleDay', state: { foo: d+"-"+this.month()+"-"+this.year()} }}>{d}</Link></span>
                </td>
            );
        }
        
        let daysInNextMonth = []; 
        var iter = 7-(daysInMonth.length+daysInPrevMonth.length)%7;
        for (let d = 1; d <= iter; d++) {
            daysInNextMonth.push(
                <td className="daysOtherMonth" key ={d+"-"+this.month()+"-"+this.year()+"nextmonth"}>
                    <span>{d}</span>
                </td>
            );
        }

        var totalSlots = [...daysInPrevMonth, ...daysInMonth, ...daysInNextMonth];
        let rows = [];
        let cells = [];

        totalSlots.forEach((row, i) => {
            if ((i % 7) !== 0) {
                cells.push(row);
            } else {
                let insertRow = cells.slice();
                rows.push(insertRow);
                cells = [];
                cells.push(row);
            }
            if (i === totalSlots.length - 1) {
                let insertRow = cells.slice();
                rows.push(insertRow);
            }
        });

        let trElems = rows.map((d, i) => {
            return (
                <tr key={i}>
                    {d}
                </tr>
            );
        })
        return (
            <div className="calendar-container" style={this.style}>
                <table className="calendar">
                    <thead>
                        <tr className="calendar-header">
                            <td className="prev" onClick={(e)=> {this.prevMonth()}}>&#10094;</td>
                            <td colSpan ="5">
                               <this.month/> - <this.year/> 
                            </td>
                            <td className="next" onClick={(e)=> {this.nextMonth()}}>&#10095;</td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td className = "week-day">S</td>
                            <td className = "week-day">M</td>
                            <td className = "week-day">T</td>
                            <td className = "week-day">W</td>
                            <td className = "week-day">T</td>
                            <td className = "week-day">F</td>
                            <td className = "week-day">Sa</td>
                        </tr>
                            {trElems}
                    </tbody>
                </table>
            </div>


            

            
        );
    }
}
