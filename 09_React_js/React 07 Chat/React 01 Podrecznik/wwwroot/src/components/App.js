import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import $ from 'jquery';
import uuid from 'uuid';
import seed, { questions } from './seed';

//react
class App extends Component {
    constructor() {
        super();
        //currentDialog - obecne pytanie, questions - array z seed.js
        this.state = {
            currentDialog: 0,
            questions: [],
            surveyState: {
                name: '',
                change: '',
                opinion: 0
            },
            tempState: {
                name: '',
                change: '',
                opinion: 0
            }
        };
    }
    
    componentDidMount() {
        //ładowanie pytań
        this.setState({ questions: seed.questions });
    }
    onInputChange = (evt) => {
        if (evt.target.name == 'name') {
            this.setState({
                tempState: {
                    name: evt.target.value,
                    change: this.state.surveyState.change,
                    opinion: this.state.surveyState.opinion
                }
            });
        };
        if (evt.target.name == 'change') {
            this.setState({
                tempState: {
                    name: this.state.surveyState.name,
                    change: evt.target.value,
                    opinion: this.state.surveyState.opinion
                }
            });
        };
        console.log('surveyState: ', this.state.surveyState);
        console.log('tempState: ', this.state.tempState);
    };
    onFormSubmit = (evt) => {
        evt.preventDefault();
        this.setState(state => ({
            surveyState: {
                ...state.surveyState,
                ...state.tempState,
            }
        }));
        if (this.state.currentDialog > 6) {
            this.setState({
                currentDialog: 8,
            });
        } else {
            this.setState({
                currentDialog: 6,
            });
        };

        console.log('currentDialog: ', this.state.currentDialog);
    };
    //kliknięcie buttonu
    onButtonClick = (evt) => {
        var elem = document.getElementById('clearMe');
        if (typeof elem !== 'undefined' && elem !== null) {
            document.getElementById("clearMe").reset();
        };
        //delkaracja
        const btn = evt.target;
        //przeskoczenie do następnego
        this.setState(
            {
                currentDialog: btn.getAttribute('goto')
            },

            () => {
                if (this.state.currentDialog == 9) {
                    //to zapisz
                    const queryParams = `Name=${this.state.surveyState.name}&Description=${this.state.surveyState.change}&Price=${this.state.surveyState.opinion}`;
                    fetch(`/comments/new?${queryParams}`)
                        .then(res => res.json())
                        .then(res => {
                            console.log(res);
                        })
                        .catch(error => {
                            console.error(error);
                        });
                    //wyzeruj stan survey
                    this.setState({
                        surveyState: {
                            name: '',
                            change: '',
                            opinion: 0
                        },
                    });

                    console.log(queryParams);
                };
                if (this.state.currentDialog == 7) {
                    this.setState({
                        surveyState: {
                            name: this.state.surveyState.name,
                            change: this.state.surveyState.change,
                            opinion: btn.getAttribute('opinion')
                        }
                    });
                };
                console.log('btn opinion:', btn.getAttribute('opinion'));
                console.log('currentDialog: ' , this.state.currentDialog);
                console.log('surveyState: ', this.state.surveyState);
                console.log('tempState: ', this.state.tempState);
            }
        );
    };
    render() {
        var replyList = questions.map(reply => {
            return (
                reply.feedback.map(singleReply => {
                    return (
                        <div>
                            {'- '}
                            <button
                                key={singleReply.id}
                                name={singleReply.name}
                                opinion={singleReply.opinion}
                                goto={singleReply.goto}
                                onClick={this.onButtonClick}>
                                {singleReply.name}
                            </button>
                        </div>
                    );
                })
            );
        });
        var write = () => {
            //argument dla input
            var toWrite = questions[this.state.currentDialog].entry;
            //jeśli jest entry'
            if (questions[this.state.currentDialog].entry)
                return (
                    <form onSubmit={this.onFormSubmit} id="clearMe">
                        {'- '}
                        <label>{toWrite.label}</label>
                        {' '}
                        <input
                            name={toWrite.name}
                            onChange={this.onInputChange}
                        />
                        {' '}
                        <input type='submit' value='Send it >'/>
                    </form>
                );
        };
        return (
            //questions - pytanie, replyList - lista odpowiedzi
            <div className="App">
                {questions[this.state.currentDialog].question}
                <br /><br />
                {write()}
                {replyList[this.state.currentDialog]}
                <br /><br />
            </div>)
    }
    
    
}
export default App;

