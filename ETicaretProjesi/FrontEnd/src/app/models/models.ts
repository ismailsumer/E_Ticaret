export interface ServiceResponse<T> {
    data: T;
    success: boolean;
    message: string;
}

export interface PagedResponse<T> extends ServiceResponse<T> {
    pageNumber: number;
    pageSize: number;
    totalRecords: number;
    totalPages: number;
}

export interface Product {
    id: number;
    name: string;
    description: string;
    price: number;
    stock: number;
    imageUrl: string;
    categoryName: string;
    brandName: string;
    categoryId: number;
    brandId: number;
}

export interface Category {
    id: number;
    name: string;
}

export interface Brand {
    id: number;
    name: string;
}

export interface BasketItem {
    id: number;
    productId: number;
    productName: string;
    price: number;
    quantity: number;
    imageUrl: string;
}

export interface OrderItem {
    productName: string;
    quantity: number;
    price: number;
}

export interface Order {
    id: number;
    orderDate: string;
    totalPrice: number;
    status: string;
    username: string;
    items: OrderItem[];
}

export interface Address {
    id: number;
    title: string;
    fullAddress: string;
    city: string;
    district: string;
    zipCode: string;
}

export interface Comment {
    id: number;
    username: string;
    content: string;
    rating: number;
    createdDate: string;
    productId: number;
    productName: string;
}

export interface Slider {
    id: number;
    title: string;
    description: string;
    imageUrl: string;
    linkUrl: string;
    isActive: boolean;
    displayOrder: number;
}

export interface LoginResponse {
    token: string;
    role: string;
    username: string;
}
