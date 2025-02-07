DROP DATABASE IF EXISTS database_name;
Create database Online_Figure_Shop;
Use Online_Figure_Shop;

create table Masters(
	`id` int auto_increment primary key,
    `category_name` nvarchar(255) not null,
    `category_id` int not null,
    `category_value` nvarchar(255) not null 
);

create table Users(
	`user_id` int auto_increment primary key,
    `role_id` int not null,
    `username` nvarchar(255) not null,
    `password` varchar(255) not null,
    `email` varchar(255) not null,
    `phone_number` varchar(15) not null,
    `address` nvarchar(255) not null,
    `dob` date not null,
    `status` int not null, -- bi cam/hoat dong/tai khoan dung hoat dong
    `modified_date` datetime(6) default null ON UPDATE CURRENT_TIMESTAMP,
	`created_date` datetime(6) not null,
    FOREIGN KEY (role_id) REFERENCES Masters(category_id),
);

create table Products (
	`product_id` int auto_increment primary key,
    `product_category_id` int not null, 
	`discount_id` int null,
    `discount_time_id` int null,
    `sku` varchar(50) not null, 
    `product_name` nvarchar(255) not null,
    `description` nvarchar(255) null, 
    `image_link` varchar(255) null, 
    `price` double not null,
    `stock_quantity` int not null,
    `stock_status` int not null, -- con hang, het hang, dung san xuat
    `created_date` DATETIME DEFAULT CURRENT_TIMESTAMP, 
    `updated_date` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP, -- When the product details were last updated
    `last_purchased_date` DATETIME NULL,
    FOREIGN KEY (product_category_id) REFERENCES Masters(category_id),
    FOREIGN KEY (discount_id) REFERENCES Masters(category_id),
    FOREIGN KEY (discount_time_id) REFERENCES Masters(category_id)
);

create table Products_Reviews(
	`review_id` int auto_increment primary key,
    `product_id` int not null,
    `user_id` int not null,
    `rating` double null,
    `comment` nvarchar(255) null,
    `image_link` varchar(255) null, 
    `uploaded_date` DATETIME DEFAULT CURRENT_TIMESTAMP, 
    `updated_date` DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP, 
    FOREIGN KEY (product_id) REFERENCES Products(product_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

create table Orders(
	`order_id` int auto_increment primary key,
    `user_ordered_id` int not null,
    `order_status_id` int not null, -- shipped, pendind, delivered, canceled
    `order_date` datetime(6) not null DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    `total_amount` double not null,
    `note` nvarchar(255) null,
    FOREIGN KEY (user_ordered_id) REFERENCES Users(category_id),
    FOREIGN KEY (order_status_id) REFERENCES Masters(category_id)
);

create table Orders_Items(
	`order_item_id` int auto_increment primary key,
    `order_id` int not null, 
    `product_id` int not null,
    `quantity` int not null,
    `price` double not null,
    FOREIGN KEY (order_id) REFERENCES Orders(order_id),
    FOREIGN KEY (product_id) REFERENCES Products(product_id)
);

CREATE TABLE Cart (
    cart_id INT AUTO_INCREMENT PRIMARY KEY, 
    user_id INT NOT NULL,
    product_id INT NOT NULL, 
    quantity INT NOT NULL DEFAULT 1, 
    created_date DATETIME DEFAULT CURRENT_TIMESTAMP, 
    updated_date DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP, 
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
    FOREIGN KEY (product_id) REFERENCES Products(product_id)
);








