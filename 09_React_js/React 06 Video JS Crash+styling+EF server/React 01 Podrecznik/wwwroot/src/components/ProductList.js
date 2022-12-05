import React, { Component } from 'react';
import $ from 'jquery';
import uuid from 'uuid';
import Product from '../components/Product';

class ProductList extends React.Component {
    deleteProduct(productID) {
        this.props.onDelete(productID);
    }
    render() {
        let productNodes;
        if (this.props.data) {
            productNodes = (this.props.data || []).map(product => {
            return (
                <Product onDelete={(e) => this.deleteProduct(product.productID)} name={product.name} key={product.productID}>
                    {product.description} <span>, </span>
                    {product.price}
                </Product>
            );
        });
    }
        return (
            <div className="productList">
                {productNodes}
            </div>
        );
    }
}
export default ProductList;