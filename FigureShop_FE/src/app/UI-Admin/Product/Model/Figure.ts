import { CategoryList } from "../../Category/Model/Category.Model";

export interface BaseProductDto {
    id: string;
    name: string;
    imageUrl?: Media[];
    price: number;
    categoryDto: CategoryList;
}

export interface ProductDetail {
    id: string;
    name: string;
    imageUrl?: Media[];
    description?: string;
    price: number;
    stockQuantity: number;
    category: CategoryList;
    categoryId?: string;
}

export interface Media {
    id: string;
    url: string;
}

export interface CartItem {
    productId: string;
    quantity: number;
    total?: number;
}


export interface ProductCreate {
    name: string;
    description?: string;
    price: number;
    stockQuantity: number;
    categoryId: string;
    media?: Media[];
}

