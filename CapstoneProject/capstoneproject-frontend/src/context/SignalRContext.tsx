import React, { createContext, useEffect, useState } from 'react';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import {LocalJwt} from "../types/AuthTypes.ts";

const BASE_SIGNALR_URL = import.meta.env.VITE_API_SIGNALR_URL;

let token = "";

export const SignalRContext = createContext<{
    orderHubConnection: HubConnection | null;
    logHubConnection: HubConnection | null;
    notificationHubConnection: HubConnection | null;
}>({
    orderHubConnection: null,
    logHubConnection: null,
    notificationHubConnection: null,
});


export const SignalRProvider: React.FC<React.PropsWithChildren> = ({ children }) => {
    const [orderHubConnection, setOrderHubConnection] = useState<HubConnection | null>(null);
    const [logHubConnection, setLogHubConnection] = useState<HubConnection | null>(null);
    const [notificationHubConnection, setNotificationHubConnection] = useState<HubConnection | null>(null);

    const jwtJson = localStorage.getItem("crawler_user");

    if (jwtJson){
        const localJwt: LocalJwt = JSON.parse(jwtJson);

        token = localJwt.accessToken;
    }

    useEffect(() => {

        const createHubConnections = async () => {

            const orderHub = new HubConnectionBuilder().withUrl(`${BASE_SIGNALR_URL}Hubs/OrderHub?access_token=${token}`).build();
            const logHub = new HubConnectionBuilder().withUrl(`${BASE_SIGNALR_URL}Hubs/LogHub?access_token=${token}`).build();
            const notificationHub = new HubConnectionBuilder().withUrl(`${BASE_SIGNALR_URL}Hubs/NotificationHub`).build();

            if (orderHub.state === 'Disconnected') {
                await orderHub.start();
            }

            if (logHub.state === 'Disconnected') {
                await logHub.start();
            }

            if (notificationHub.state === 'Disconnected') {
                await notificationHub.start();
            }

            setOrderHubConnection(orderHub);
            setLogHubConnection(logHub);
            setNotificationHubConnection(notificationHub);
        };

        createHubConnections();

        // Cleanup the hub connections on unmount
        return () => {
            orderHubConnection?.stop();
            logHubConnection?.stop();
            notificationHubConnection?.stop();
        };
    }, []);

    // Provide the hub connections through the SignalR context
    return (
        <SignalRContext.Provider value={{ orderHubConnection, logHubConnection, notificationHubConnection }}>
            {children}
        </SignalRContext.Provider>
    );
};
