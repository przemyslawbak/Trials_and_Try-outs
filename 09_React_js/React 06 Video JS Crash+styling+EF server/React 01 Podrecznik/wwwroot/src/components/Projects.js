import React, { Component } from 'react';
import $ from 'jquery';
import ProjectItem from './ProjectItem';


//react
class Projects extends Component {
    deleteProject(id) {
        this.props.onDelete(id)
    }
    render() {
        let projectItems;
        if (this.props.projects) {
            projectItems = this.props.projects.map(project => {
                return (
                    <ProjectItem onDelete={this.deleteProject.bind(this)} key={project.title} project={project}/>
                    );
            });
        }
        //wszystko dla 'return' musi być w jednym elemencie <div>
        return (
            <div className="Projects">
                {projectItems}
            </div>
        );
    }
}
export default Projects;

//jquery