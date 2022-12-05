import React, { Component } from 'react';
import $ from 'jquery';


//react
class ProjectItem extends Component {
    deleteProject(id) {
        this.props.onDelete(id);
    }
    render() {
        //wszystko dla 'return' musi być w jednym elemencie <div>
        return (
            <li className="Project">
                <strong>{this.props.project.title}</strong> - {this.props.project.category} <a href="#" onClick={this.deleteProject.bind(this, this.props.project.id)}>X</a>
            </li>
        );
    }
}
export default ProjectItem;

//jquery