import { CategoryList } from "../../Category/Model/Category.Model";

export interface BaseProductDto {
    id: string;
    name: string;
    imageUrl?: Media[];
    price: number;
    category: CategoryList;
}

export interface ProductDetail {
    id: string;
    name: string;
    imageUrl?: Media[];
    description?: string;
    price: number;
    stockQuantity: number;
    category: CategoryList;
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