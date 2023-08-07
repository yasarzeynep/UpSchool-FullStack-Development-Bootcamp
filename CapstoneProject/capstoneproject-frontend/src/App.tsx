import './App.css'
import NavBar from './components/NavBar.tsx';
import {ToastContainer} from 'react-toastify';
import {Container} from "semantic-ui-react";
import {Route, Routes} from "react-router-dom";
import NotFoundPage from "./pages/NotFoundPage.tsx";
import {useEffect, useState} from "react";
import {LocalJwt, LocalUser} from "./types/AuthTypes.ts";
import LoginPage from "./pages/LoginPage.tsx";
import {getClaimsFromJwt} from "./utils/jwtHelper.ts";
import {useNavigate} from "react-router-dom";
import {AppUserContext } from "./context/StateContext.tsx";
import ProtectedRoute from "./components/ProtectedRoute.tsx";
import SocialLogin from "./pages/SocialLogin.tsx";
import CrawlerLivePage from './pages/CrawlerLivePage.tsx';

import OrdersPage from './pages/OrdersPage.tsx'; //
import SettingsPage from './pages/SettingsPage.tsx'; //
import UsersPage from './pages/UsersPage.tsx'; //
import ProductsPage from './pages/ProductsPage.tsx'; //

function App() {

    const navigate = useNavigate();

    const [appUser, setAppUser] = useState<LocalUser | undefined>(undefined);

    useEffect(() => {

        const jwtJson = localStorage.getItem("crawler_user");

        if (!jwtJson) {

            navigate("/login");
            return;

        }

        const localJwt: LocalJwt = JSON.parse(jwtJson);

        const { uid, email, given_name, family_name } = getClaimsFromJwt(localJwt.accessToken);

        const expires: string = localJwt.expires;

        setAppUser({ id: uid, email, firstName: given_name, lastName: family_name, expires, accessToken: localJwt.accessToken });

    }, []);

    return (
        <>
            <AppUserContext.Provider value={{appUser, setAppUser}}>

                    <ToastContainer/>
                    <NavBar />
                    <Container className="App">
                        <Routes>

                            <Route path="/crawlerLive" element={
                                <ProtectedRoute>
                                    <CrawlerLivePage />
                                </ProtectedRoute>
                            }/>

                            <Route path="/login" element={<LoginPage/>}/>
                            <Route path="/social-login" element={<SocialLogin/>}/>
                            <Route path="/orders" element={<ProtectedRoute><OrdersPage /></ProtectedRoute>} />
                            <Route path="/settings" element={<ProtectedRoute><SettingsPage /></ProtectedRoute>} />
                            <Route path="/users" element={<ProtectedRoute><UsersPage /></ProtectedRoute>} />
                            <Route path="/products" element={<ProtectedRoute><ProductsPage /></ProtectedRoute>} />
                            <Route path="*" element={<NotFoundPage/>}/>
                        </Routes>
                    </Container>

            </AppUserContext.Provider>
        </>
    )

}


export default App


