import React, { useEffect, useState } from 'react';
import { HubConnectionBuilder } from '@microsoft/signalr';
import axios from 'axios';
import { OrderDto, ProductCrawlType } from '../types/OrderTypes';


const OrdersPage: React.FC = () => {
    const [orders, setOrders] = useState<OrderDto[]>([]);

    useEffect(() => {
        const newConnection = new HubConnectionBuilder()
            .withUrl(`${import.meta.env.VITE_API_SIGNALR_URL}/Hubs/OrderHub`)
            .build();

        newConnection
            .start()
            .then(() => console.log('SignalR connected.'))
            .catch((error) => console.error('SignalR connection error: ', error));

        return () => {
            newConnection.stop();
        };
    }, []);

    useEffect(() => {
        fetchOrders();
    }, []);

    const fetchOrders = async () => {
        try {
            const backendUrl = import.meta.env.VITE_API_URL;
            const response = await axios.get<OrderDto[]>(`${backendUrl}/api/Orders`);

            if (response.status === 200) {
                setOrders(response.data);
            }
        } catch (error) {
            console.error('Error fetching orders:', error);
        }
    };

    return (
        <div className="OrdersPage">
            <h1>Orders</h1>
            <table>
                <thead>
                <tr>
                    <th>ID</th>
                    <th>Requested Amount</th>
                    <th>Total Found Amount</th>
                    <th>Product Crawl Type</th>
                    <th>Is Deleted</th>
                </tr>
                </thead>
                <tbody>
                {orders.map((order) => (
                    <tr key={order.id}>
                        <td>{order.id}</td>
                        <td>{order.requestedAmount}</td>
                        <td>{order.totalFoundAmount}</td>
                        <td>{ProductCrawlType[order.productCrawlType]}</td>
                        <td>{order.isDeleted ? 'Yes' : 'No'}</td>
                    </tr>
                ))}
                </tbody>
            </table>
        </div>
    );
};

export default OrdersPage;
