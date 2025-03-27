import { InterpolationConfig } from "@angular/compiler";

export interface Order {
    id: string;
    userId: string;
    user?: User | null; // API có thể trả về null
    status: number;
    orderDate: string;  // API trả về dạng ISO string
    totalAmount: number;
    note: string;
    items: OrderItem[];
    createdAt: string;
    createdBy: string;
    lastModifiedAt?: string | null;
    lastModifiedBy?: string | null;
}

export interface OrderItem {
    id: string;
    orderId: string;
    productId: string;
    quantity: number;
    total: number;
    product: ProductOrder;
}

export interface ProductOrder {
    id: string;
    name: string;
    description: string;
    price: number;
    imageUrl: string; // Lấy ảnh đầu tiên từ `media`
}

export interface User {
    id: string;
    email?: string;
    userName?: string;
}

export interface ProductCheckout {
    id: string,
    image: string,
    productName: string,
    price: number,
    quantity: number,
}

export interface ListOrder {
    userId: string;
    listOrder: ProductCheckout[]
}
