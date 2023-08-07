
import {Container, Menu, Image, Icon, Button} from "semantic-ui-react";

import {NavLink, useNavigate} from "react-router-dom";
import {useContext} from "react";
import { AppUserContext} from "../context/StateContext.tsx";


/*export type NavbarProps = {

}*/

const NavBar = () => {
    const {appUser, setAppUser} = useContext(AppUserContext);

    const navigate = useNavigate();

    const handleLogout = () => {

        localStorage.removeItem("crawler_user");

        setAppUser(undefined);

        navigate("/login");

    }
// <Image size='mini' src='/vite.svg' style={{ marginRight: '1.5em' }} />
    return (
        <Menu fixed='top' inverted>
            <Container>
                <Menu.Item as='a' header>
                    <Image size='mini' src='/logo_new.png' style={{ marginRight: '1.5em' }} />
                    Data Crawler
                </Menu.Item>
                <Menu.Item as={NavLink} to="/">Home</Menu.Item>
                <Menu.Item as={NavLink} to='/users'>Users</Menu.Item>
                <Menu.Item as={NavLink} to='/orders'>Orders</Menu.Item>
                <Menu.Item as={NavLink} to='/produtcs'>Produtcs</Menu.Item>
                <Menu.Item as={NavLink} to="/crawlerLive">Crawler Live</Menu.Item>
                <Menu.Item as={NavLink} to='/settings'>Settings</Menu.Item>
                <Menu.Item as={NavLink} to="/dafasdqweasdaf">Not Found</Menu.Item>
                {!appUser && <Menu.Item as={NavLink} to="/login" position="right"><Icon name="sign-in" /> Login</Menu.Item>}
                {appUser && <Menu.Item as={Button} onClick={handleLogout} position="right"><Icon name="sign-out" /> Logout</Menu.Item>}
            </Container>
        </Menu>
    );
}

export default  NavBar;