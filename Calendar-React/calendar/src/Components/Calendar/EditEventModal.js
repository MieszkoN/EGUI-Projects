import React, {Component} from 'react';
import {Modal, Button, Row, Col, Form} from 'react-bootstrap';

export class EditEventModal extends Component {
    constructor(props) {
        super(props);
        this.wrapper = React.createRef();
    }


    handleSubmit(event) {
      event.preventDefault();
      let se = {
        id: parseInt(event.target.eventid.value),
        timeOfEvent: event.target.timeOfEvent.value,
        description: event.target.description.value
      }
      fetch('api/allEvents', {
        method: 'PUT',
        headers:{
          'Accept': 'application/json',
          'Content-Type':'application/json'
        },
        body:JSON.stringify(
          {
          id: parseInt(event.target.eventid.value),
          timeOfEvent: event.target.timeOfEvent.value,
          description: event.target.description.value
          })
      })
      .then(response=>response.json())
      console.log(se);
      

    }

    render() {
        return(
          <div>
            <Modal
            {...this.props}
            size="lg"
            aria-labelledby="contained-modal-title-vcenter"
            centered
            >
            <Modal.Header closeButton>
              <Modal.Title id="contained-modal-title-vcenter">
                Edit Event
              </Modal.Title>
            </Modal.Header>
            <Modal.Body>
              <div className="container">
                  <Row>
                    <Col sm={6}>
                      <Form onSubmit={this.handleSubmit}>
                      <Form.Group controlId="eventid">
                          <Form.Control type="hidden" name="eventid" defaultValue={this.props.eventid}/>  
                        </Form.Group>
                        <Form.Group controlId="timeOfEvent">
                          <Form.Label>Time: </Form.Label>
                          <Form.Control type="time" name="timeOfEvent" defaultValue={this.props.time} required/>  
                        </Form.Group>
                        <Form.Group controlId="description">
                          <Form.Label>Description: </Form.Label>
                          <Form.Control type="text" name="description" defaultValue={this.props.des} required/>  
                        </Form.Group>  

                        <Form.Group>
                          <Button variant="primary" type="submit" onClick={this.props.onHide}>
                            Save
                          </Button>
                        </Form.Group> 
                      </Form>
                    </Col>
                  </Row>
              </div>

            </Modal.Body>
            <Modal.Footer>
              <Button variant="danger" onClick={this.props.onHide}>Close</Button>
            </Modal.Footer>
          </Modal>
          </div>
        );
    }
}