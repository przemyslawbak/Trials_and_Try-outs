import React, { Component } from 'react';
import $ from 'jquery';
import uuid from 'uuid';

class ProductForm extends React.Component {
    constructor(props) {
        super(props);
        this.state = { name: '', description: '' };
        this.handleNameChange = this.handleNameChange.bind(this);
        this.handleDescriptionChange = this.handleDescriptionChange.bind(this);
        this.handleSubmit = this.handleSubmit.bind(this);
    }
    handleNameChange(e) {
        this.setState({ name: e.target.value });
    }
    handleDescriptionChange(e) {
        this.setState({ description: e.target.value });
    }
    handleSubmit(e) {
        e.preventDefault();
        const name = this.state.name.trim();
        const description = this.state.description.trim();
        if (!description || !name) {
            return;
        }
        this.props.onProductSubmit({ Name: name, Description: description });
        this.setState({ name: '', description: '' });
    }
    render() {
        return (
            <form className="productForm" onSubmit={this.handleSubmit}>
                <input
                    type="text"
                    placeholder="Product name"
                    value={this.state.name}
                    onChange={this.handleNameChange}
                />
                <input
                    type="text"
                    placeholder="Product description"
                    value={this.state.description}
                    onChange={this.handleDescriptionChange}
                />
                <input type="submit" value="Post" />
            </form>
        );
    }
}

export default ProductForm;