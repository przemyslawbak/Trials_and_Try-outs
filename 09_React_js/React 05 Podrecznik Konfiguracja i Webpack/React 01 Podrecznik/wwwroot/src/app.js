import React from 'react';
import ReactDOM from 'react-dom';
import Counter from '../src/reactcomponent';
import $ from 'jquery';
import FetchData from '../src/fetchdata';
import ES6Lib from '../src/es6codelib';


//react

ReactDOM.render(
    <FetchData />,
    document.getElementById('reactcomponentwithapidata')
);

ReactDOM.render(
    <Counter />,
    document.getElementById('basicreactcomponent')
);

ReactDOM.render(
    <h1>
         Witaj w aplikacji, luju!
</h1>,
    document.getElementById('app')

);

//jquery

$('#fillthiswithjquery').html('Filled by Jqueryy!'); 