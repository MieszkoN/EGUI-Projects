import React, {Component} from 'react';
import {Modal, Button, Row, Col, Form} from 'react-bootstrap';

export class SingleEventModal extends Component {
    constructor(props) {
        super(props);
        this.wrapper = React.createRef();
    }


    handleSubmit(event) {
      event.preventDefault();
      let se = {
        timeOfEvent: event.target.timeOfEvent.value,
        dateOfEvent: event.target.foo.value,
        description: event.target.description.value
      }
      fetch('api/allEvents', {
        method: 'POST',
        headers:{
          'Accept': 'application/json',
          'Content-Type':'application/json'
        },
        body:JSON.stringify(se)
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
                Single Event Page
              </Modal.Title>
            </Modal.Header>
            <Modal.Body>
              <div className="container">
                  <Row>
                    <Col sm={6}>
                      <Form onSubmit={this.handleSubmit}>
                      <Form.Group controlId="foo">
                          <Form.Control type="hidden" name="foo" defaultValue={this.props.foo}/>  
                        </Form.Group>

                        <Form.Group controlId="timeOfEvent">
                          <Form.Label>Time: </Form.Label>
                          <Form.Control type="time" name="timeOfEvent" required/>  
                        </Form.Group>
                        <Form.Group controlId="description">
                          <Form.Label>Description: </Form.Label>
                          <Form.Control type="text" name="description" required/>  
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