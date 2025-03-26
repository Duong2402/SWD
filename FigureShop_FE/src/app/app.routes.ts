import { Routes } from '@angular/router';

import { HomePageComponent } from './core/home-page/home-page.component';
import { ProductDetailComponent } from './UI-Client/product-detail/product-detail.component';
import { CartComponent } from './UI-Client/cart/cart.component';
import { CheckoutComponent } from './UI-Client/checkout/checkout.component';
import { LoginComponent } from './core/login/login.component';
import { SignUpComponent } from './core/sign-up/sign-up.component';
import { ListProductComponent } from './UI-Client/list-product/list-product.component';
import { ProductListComponent } from './UI-Admin/Product/product-list/product-list.component';
import { OrderListComponent } from './UI-Admin/Order/order-list/order-list.component';
import { OrderDetailComponent } from './UI-Admin/Order/order-detail/order-detail.component';
import { ProductCreateComponent } from './UI-Admin/Product/product-create/product-create.component';
import { ProductDetailAdminComponent } from './UI-Admin/Product/product-detail/product-detail.component';
import { HistoryOrderComponent } from './UI-Client/history-order/history-order.component';
import { ListCategoryComponent } from './UI-Admin/Category/list-category/list-category.component';
import { ResultComponent } from './core/search-result/result/result.component';

export const routes: Routes = [
    {
        path: '',
        component: HomePageComponent
    },
    {
        path: 'shop',
        component: ListProductComponent,
    },
    {
        path: 'shop/:id',
        component: ProductDetailComponent
    },
    {
        path: 'cart',
        component: CartComponent
    },
    {
        path: 'checkout',
        component: CheckoutComponent
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'register',
        component: SignUpComponent
    },
    {
        path: 'admin/products',
        component: ProductListComponent
    },
    {
        path: 'admin/products/create',
        component: ProductCreateComponent
    },
    {
        path: 'admin/products/:id',
        component: ProductDetailAdminComponent
    },
    {
        path: 'admin/orders',
        component: OrderListComponent
    },
    {
        path: 'order/:id',
        component: OrderDetailComponent
    },
    {
        path: 'booking',
        component: HistoryOrderComponent
    },
    {
        path: 'admin/categories',
        component: ListCategoryComponent
    },
    {
        path: 'search/',
        component: ResultComponent
    }
];

