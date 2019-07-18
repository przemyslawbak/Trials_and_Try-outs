import React, { Component } from 'react';
import $ from 'jquery';
import uuid from 'uuid';

class Product extends React.Component {
    deleteProduct(productID) {
        this.props.onDelete(productID);
    }
    render() {
        return (
            <div className="product">
                <h2 className="productName">
                    {this.props.name} <button href="#" onClick={this.deleteProduct.bind(this, this.props.productID)}> DELETE </button>
                </h2>
                {this.props.children}
            </div>
        );
    }
}
export default Product;