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
    imageUrl?: string;
    vendors?: string;
    type?: string;
    price: number;
    category: CategoryList;
}

export interface Media {
    id: string;
    url: string;
}