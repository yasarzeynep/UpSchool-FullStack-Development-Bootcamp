export enum ProductCrawlType {
    All = 1,
    OnDiscount = 2,
    NonDiscount = 3,
}

export enum ProductRequestedAmount {
    All = 1,
    SpecificAmount = 2,
}

export interface ProductDto {
    id: string;
    orderId: string;
    name: string;
    picture: string;
    isOnSale: boolean;
    price: number;
    salePrice?: number;
    isDeleted: boolean;
}

export type ProductGetAllQuery = {
    isDeleted: boolean;
};

export interface OrderDto {
    id: string;
    requestedAmount: number;
    totalFoundAmount: number;
    productCrawlType: ProductCrawlType;
    isDeleted: boolean;
}

export type OrderGetAllQuery = {
    isDeleted: boolean;
};

export interface OrderEventsDto {
    orderId: string;
    status: string;
    isDeleted: boolean;
}

export type OrderEventGetAllQuery = {
    isDeleted: boolean;
};

export type OrderAddCommand = {
    requestedAmount: number;
    productCrawlType: ProductCrawlType;
    email: string;
    name: string;
};
