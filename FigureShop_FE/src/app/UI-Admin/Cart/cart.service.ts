import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { URL_Base } from '../../app.config';
import { Observable } from 'rxjs/internal/Observable';
import { CartItem } from '../../UI-Admin/Cart/Model/CartItem ';

@Injectable({
    providedIn: 'root'
})
export class CartService {
    private api = `${URL_Base}/Cart`;
    constructor(private http: HttpClient) { }

    getCart(userId: string): Observable<CartItem[]> {
        return this.http.get<CartItem[]>(`${this.api}/GetCart`, {
            params: { userId: userId },
        });
    }

    addToCart(userId: string, productId: string, quantity: number): Observable<any> {
        const cartDTO = {
            userId: userId,
            productId: productId,
            quantity: quantity
        };
        return this.http.post(`${this.api}/AddToCart`, cartDTO);
    }
    removeFromCart(cartId: string, productId: string): Observable<any> {
        return this.http.delete(`${this.api}/RemoveFromCart`, {
            params: { cartId, productId },
        });
    }
    updateQuantity(userId: string, productId: string, quantity: number): Observable<any> {
        const cartDTO = {
            userId: userId,
            productId: productId,
            quantity: quantity
        };
        return this.http.put(`${this.api}/UpdateQuantity`, cartDTO);
    }
}