import React, { useContext, useState } from 'react';
import { AuthRegisterCommand, LocalJwt } from '../types/AuthTypes';
import { toast } from 'react-toastify';
import { getClaimsFromJwt } from '../utils/jwtHelper';
import { useNavigate } from 'react-router-dom';
import { RegisterUserContext } from '../context/StateContext';
import api from "../utils/axiosInstance.ts";
import { Form, Button, Grid, Header, Image, Message, Segment } from 'semantic-ui-react';

export default function RegisterPage() {
    const { setRegisterUser } = useContext(RegisterUserContext);
    const navigate = useNavigate();
    const [registerCommand, setRegisterCommand] = useState<AuthRegisterCommand>({
        firstName: '',
        lastName: '',
        email: '',
        password: ''
    });

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault();
        try {
            const response = await api.post("/Authentication/Register", registerCommand);
            if (response.status === 200) {
                const accessToken = response.data.accessToken;
                const { uid, email, given_name, family_name } = getClaimsFromJwt(accessToken);
                const expires: string = response.data.expires;
                setRegisterUser({ id: uid, email, firstName: given_name, lastName: family_name, expires, accessToken });
                const localJwt: LocalJwt = {
                    accessToken,
                    expires
                }
                localStorage.setItem("crawler_user", JSON.stringify(localJwt));
                navigate("/login");
                toast.success("Registration successful!");
            }
        } catch (error) {
            toast.error("Registration failed!");
        }
    };

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setRegisterCommand({
            ...registerCommand,
            [event.target.name]: event.target.value
        });
    };

    return (
        <Grid textAlign='center' style={{ height: '100vh' }} verticalAlign='middle'>
            <Grid.Column style={{ maxWidth: 450 }}>
                <Header as='h2' color='teal' textAlign='center'>
                    <Image src='/logo.png' /> Register Here!
                </Header>
                <Form size='large' onSubmit={handleSubmit}>
                    <Segment stacked>
                        <Form.Input
                            fluid
                            icon='user'
                            iconPosition='left'
                            placeholder='First Name'
                            name='firstName'
                            value={registerCommand.firstName}
                            onChange={handleInputChange}
                            required
                        />
                        <Form.Input
                            fluid
                            icon='user'
                            iconPosition='left'
                            placeholder='Last Name'
                            name='lastName'
                            value={registerCommand.lastName}
                            onChange={handleInputChange}
                            required
                        />
                        <Form.Input
                            fluid
                            icon='mail'
                            iconPosition='left'
                            placeholder='Email'
                            type='email'
                            name='email'
                            value={registerCommand.email}
                            onChange={handleInputChange}
                            required
                        />
                        <Form.Input
                            fluid
                            icon='lock'
                            iconPosition='left'
                            placeholder='Password'
                            type='password'
                            name='password'
                            value={registerCommand.password}
                            onChange={handleInputChange}
                            required
                        />

                        <Button color='teal' fluid size='large' type="submit">
                            Register
                        </Button>
                    </Segment>
                </Form>
                <Message>
                    Already have an account? <a href='/login'>Login</a>
                </Message>
            </Grid.Column>
        </Grid>
    );
}

