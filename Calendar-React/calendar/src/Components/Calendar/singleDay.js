import React,{Component} from 'react';
import {Table} from 'react-bootstrap';
import {Button, ButtonToolbar} from 'react-bootstrap';
import {SingleEventModal} from './SingleEventModal';
import {EditEventModal} from './EditEventModal';
import './calendar.css';

export class SingleDay extends Component {

    constructor() {
        super();
        this.state={allEvents:[], addModalShow: false, editModalShow: false}
    }

    refreshList() {
        fetch('http://localhost:5000/api/allEvents')
        .then(response=>response.json())
        .then(data => {
            this.setState({allEvents:data})
        });
    }

    componentDidMount() {
        this.refreshList();
    }

    componentDidUpdate() {
        this.refreshList();
    }


    deleteEvent(se) {
          fetch('api/allEvents', {
            method: 'DELETE',
            headers:{
              'Accept': 'application/json',
              'Content-Type':'application/json'
            },
            body:JSON.stringify(
            {
                'id': se.id
            })
          })
          .then(response=>response.json())
          console.log(se);
    }


    render() {
        const {foo} = this.props.location.state
        const {allEvents, eventid, time, des} = this.state;
        let addModalClose = () => this.setState({addModalShow:false});
        let editModalClose = () => this.setState({editModalShow:false});

        let eventsInDay = []
        for (let i = 1; i <= allEvents.size; i++) {
            if(allEvents[i].dateOfEvent === foo) {
                eventsInDay.push(allEvents(i));
            }
        }

        //eslint-disable-next-line
        let row = allEvents.map(singleEvent =>{
            if(singleEvent.dateOfEvent===foo) {
                return (
                    <tr key = {singleEvent.id}>
                    <td>{singleEvent.timeOfEvent}</td>
                    <td>{singleEvent.description}</td>
                    <td><ButtonToolbar>
                            <Button className="mr-2" variant="info" onClick= {()=> this.setState({editModalShow:true, eventid:singleEvent.id, time:singleEvent.timeOfEvent, des:singleEvent.description})}>
                                Edit
                            </Button>
                            <EditEventModal show ={this.state.editModalShow} onHide={editModalClose} eventid = {eventid} time={time} des={des}/>
                            <Button className="mr-2" variant="danger" onClick= {()=> this.deleteEvent(singleEvent)}>
                                Delete
                            </Button>
                        </ButtonToolbar>
                    </td>
                    </tr>
                )
            }
        }
           
            )


        return( 
            <div>
                <h2>Single Day Page</h2>
        <h3> {foo}</h3>
                <Table className ="mt-4" striped bordered hover size="sm">
                   <thead>
                        <tr>
                            <th>Time</th>
                            <th>Description</th>
                            <th>Edit Event/Delete</th>
                        </tr>
                    </thead> 
                    <tbody>
                        {row}

                    </tbody>
                </Table>
                
                <ButtonToolbar>
                    &nbsp;
                    <Button variant='primary' onClick={()=>this.setState({addModalShow: true})}>Add New</Button>

                    <SingleEventModal show={this.state.addModalShow} onHide={addModalClose} foo={foo}/>
                    &nbsp;&nbsp;&nbsp;
                    <a className="btn btn-danger" href="/" role="button">Close</a>
                </ButtonToolbar>
                </div>
        )
    }

}