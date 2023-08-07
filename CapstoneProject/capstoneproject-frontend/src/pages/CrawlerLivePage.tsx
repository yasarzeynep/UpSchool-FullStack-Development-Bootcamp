import { useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { Container, Header } from 'semantic-ui-react';
import { SeleniumLogDto } from '../types/CrawlerTypes';
import { addLog } from '../store/features/crawlerLive/crawlerLiveSlice';
import './CrawlerLivePage.css';
import { RootState } from '../store/store';

const VITE_SIGNALR_URL = import.meta.env.VITE_SIGNALR_URL;

const CrawlerLivePage = () => {
    const dispatch = useDispatch();
    const logs = useSelector((state: RootState) => state.crawlerLive.logs);

    useEffect(() => {
        const hubConnection = new HubConnectionBuilder()
            .withUrl(`${VITE_SIGNALR_URL}/Hubs/SeleniumLogHub`)
            .withAutomaticReconnect()
            .build();

        hubConnection.on('NewCrawlerLogAdded', (seleniumLogDto) => {
            dispatch(addLog(seleniumLogDto));
        });

        hubConnection.start();

        return () => {
            hubConnection.stop();
        };
    }, [dispatch]);

    return (
        <Container>
            <Header as="h3">CrawlerLive</Header>
            <div className="fakeMenu">
                <div className="fakeButtons fakeClose"></div>
                <div className="fakeButtons fakeMinimize"></div>
                <div className="fakeButtons fakeZoom"></div>
            </div>
            <div className="fakeScreen">
                {logs.map((log: SeleniumLogDto, index: number) => (
                    <p key={index} className="line1">
                        {log.message} | {new Date(log.sentOn).toLocaleString('en-US', { hour12: true })}
                    </p>
                ))}
            </div>
        </Container>
    );
};

export default CrawlerLivePage;