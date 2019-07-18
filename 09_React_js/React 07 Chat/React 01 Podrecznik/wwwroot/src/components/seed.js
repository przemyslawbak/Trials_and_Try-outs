import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import $ from 'jquery';
import uuid from 'uuid';

var currentHour = new Date().getHours();
var timeOfDay = () => {
    if (currentHour > 0 && currentHour < 12) return ('morning');
    if (currentHour > 12 && currentHour < 19) return ('afternoon');
    if (currentHour > 19 && currentHour < 24) return ('evening');
};
export const questions = [
    {
        //[0] greetings
        id: uuid.v4(),
        question: 'Welcome to my website! I hope you are enjoy your ' + timeOfDay() + '?',
        feedback: [
            { name: 'Yes, it is pretty nice indeed.', goto: 1, id: uuid.v4() },
            { name: 'I have nothing to complain about.', goto: 1, id: uuid.v4() },
            { name: 'It used to be better.', goto: 2, id: uuid.v4() }
        ]
    },
    {
        //[1] greetings good react
        id: uuid.v4(),
        question: 'Great to hear that.',
        feedback: [
            { name: 'Continue >', goto: 3, id: uuid.v4() }
        ]
    },
    {
        //[2] greetings bad react
        id: uuid.v4(),
        question: 'I am sorry to hear that.',
        feedback: [
            { name: 'Continue >', goto: 3, id: uuid.v4() }
        ]
    },
    {
        //[3]
        id: uuid.v4(),
        question: 'Let me introduce myself. My name is Przemek, and I builded this website. What is your name?',
        feedback: [
            { name: 'I am not sure that I am supposed to andswer that question...', goto: 4, id: uuid.v4() },
            { name: 'I dont like to talk to strangers. What do you need my name for?', goto: 5, id: uuid.v4() }
        ],
        entry: { label: 'My name is', name: 'name', id: uuid.v4() }
    },
    {
        //[4]
        id: uuid.v4(),
        question: 'Dont be so shy! I like to meet new people. And I promisse you, I will not sell this data to chinese mafia.',
        feedback: [
            { name: 'I will keep it in secret.', goto: 6, id: uuid.v4() }
        ],
        entry: { label: 'My name is', name: 'name', id: uuid.v4() }
    },
    {
        //[5]
        id: uuid.v4(),
        question: 'Of corse, you do not have to tell me, I just like to know who am I talking with.',
        feedback: [
            { name: 'Still I do not feel convinced.', goto: 6, id: uuid.v4() }
        ],
        entry: { label: 'My name is', name: 'name', id: uuid.v4() }
    },
    {
        //[6]
        id: uuid.v4(),
        question: '<name>, tell me, what do you think about my website?',
        feedback: [
            { name: 'Actually I really like it! You did a great job.', goto: 7, opinion: 5, id: uuid.v4() },
            { name: 'It is pretty cool, I like it.', goto: 7, opinion: 4, id: uuid.v4() },
            { name: 'The website is not bad. But I would change few things.', goto: 7, opinion: 3, id: uuid.v4() },
            { name: 'I saw better.', goto: 7, opinion: 2, id: uuid.v4() },
            { name: 'It is one of the worst I have ever seen.', goto: 7, opinion: 1, id: uuid.v4() }
        ]
    },
    {
        //[7]
        id: uuid.v4(),
        question: 'Is there anything you would change here?',
        feedback: [
            { name: 'No, actually I have nothing in my mind right now', goto: 8, id: uuid.v4() }
        ],
        entry: { label: 'You could change', name: 'change', id: uuid.v4() }
    },
    {
        //[8] greetings bad react
        id: uuid.v4(),
        question: 'Great, I appreciate your feedback. That really means a lot to me.',
        feedback: [
            { name: 'Continue >', goto: 9, id: uuid.v4() }
        ]
    },
    {
        //[9]
        id: uuid.v4(),
        question: 'If you would like to ask me some questions, call/wite me.',
        feedback: []
        
    }
]

export default {
    questions
}