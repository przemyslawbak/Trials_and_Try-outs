import React, { Component } from 'react';
import $ from 'jquery';
import ProductList from '../components/ProductList';
import ProductForm from '../components/ProductForm';

class ProductBox extends React.Component {
    constructor(props) {
        super(props);
        this.state = { data: this.props.initialData };
        this.handleProductSubmit = this.handleProductSubmit.bind(this);
        this.handleDeleteProject = this.handleDeleteProject.bind(this);
    }
    componentDidMount() {
        window.setInterval(() => this.loadProductsFromServer(), this.props.pollInterval);
    }
    loadProductsFromServer() {
        const xhr = new XMLHttpRequest();
        xhr.open('get', this.props.url, true);
        xhr.onload = () => {
            const data = JSON.parse(xhr.responseText);
            this.setState({ data: data });
        };
        xhr.send();
    }
    handleProductSubmit(product) {
        const queryParams = `Name=${product.Name}&Description=${product.Description}`;
        fetch(`/comments/new?${ queryParams }`)
            .then(res => res.json())
            .then(res => {
                console.log(res);
            })
            .catch(error => {
                console.error(error);
            });
    }
    handleDeleteProject(projectID) {
        
        fetch("/comments/delete?productID=" + encodeURIComponent(projectID))
            .then(res => res.json())
            .then(res => {
                console.log(res);
            })
            .catch(error => {
                console.error(error);
            });
    }
    render() {
        return (
            <div className="productBox">
                <h1>Tutaj React z EF</h1>
                <ProductForm onProductSubmit={this.handleProductSubmit} />
                <ProductList onDelete={this.handleDeleteProject} data={this.state.data} />
            </div>
        );
    }
}

export default ProductBox;