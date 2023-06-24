export interface Product {
    id: number;
    descriptions: string;
    qty: number;
    unit: string;
    cost_price: number;
    sell_price: number;
    prod_pic: string;
    category: string;
    sale_price: number;
    alert_level: number;
    critical_level: number;
    datecreated: Date;
    dateupdated: Date;
}
