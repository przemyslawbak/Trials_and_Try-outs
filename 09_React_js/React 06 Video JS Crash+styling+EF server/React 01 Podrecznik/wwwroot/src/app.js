import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import $ from 'jquery';
import uuid from 'uuid';
import Projects from '../src/components/Projects';
import ProjectItem from '../src/components/ProjectItem';
import AddProject from '../src/components/AddProject';
import ProductBox from '../src/components/ProductBox';


//react
class App extends Component {
    constructor() {
        super();
        this.state = {
            projects: []
        }
    }
    //lifecycle method
    //fires off every time when component is re-rendered
    componentWillMount() {
        this.setState({
            projects: [
            //id z uuid
                {
                    id: uuid.v4(),
                    title: 'Business website',
                    category: 'Web design'
                },
                {
                    id: uuid.v4(),
                    title: 'Social App',
                    category: 'Mobile Development'
                },
                {
                    id: uuid.v4(),
                    title: 'Ecommerce Shopping Cart',
                    category: 'Web development'
                }
            ]
        });
    }

    handleAddProject(project) {
        let projects = this.state.projects;
        projects.push(project);
        this.setState({ projects: projects });
    }

    handleDeleteProject(id) {
        let projects = this.state.projects;
        let index = projects.findIndex(x => x.id === id);
        projects.splice(index, 1)
        this.setState({ projects: projects });
    }

    render() {
    //wszystko dla 'return' musi być w jednym elemencie <div>

        //wszystko do 'Projects' przesyłamy przez własności typu 'test' albo 'projects'
        return (
            <div className="App">
                    Welcome to React!
                    <AddProject addProject={this.handleAddProject.bind(this)}/>
                    <Projects onDelete={this.handleDeleteProject.bind(this)} projects={this.state.projects} />
                    <ProductBox url="/comments" submitUrl="/comments/new" deleteUrl="/comments/delete" pollInterval={2000}/>
            </div>
        );
    }
}
export default App;

//jquery
