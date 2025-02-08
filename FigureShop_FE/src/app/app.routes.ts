import { Routes } from '@angular/router';
import { ListProductComponent } from './UI-Client/list-product/list-product.component';
import { HomePageComponent } from './core/home-page/home-page.component';
import { ProductDetailComponent } from './UI-Client/product-detail/product-detail.component';
import { CartComponent } from './UI-Client/cart/cart.component';
import { CheckoutComponent } from './UI-Client/checkout/checkout.component';
import { LoginComponent } from './core/login/login.component';
import { SignUpComponent } from './core/sign-up/sign-up.component';

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
        path: 'shop/id',
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
    }
];

