import { CategoryList } from "../../Category/Model/Category.Model";

export interface BaseProductDto {
    name: string;
    imageUrl?: string;
    vendors?: string;
    type?: string;
    price: number;
    category: CategoryList;

}