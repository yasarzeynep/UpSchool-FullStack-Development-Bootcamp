import React, { useEffect, useState } from "react";
import { Table, Header } from "semantic-ui-react";
import axios from "axios";
import { ApiResponse, LocalUser } from "../types/AuthTypes";

const BASE_URL = "https://localhost:7245/api";

const UsersPage = () => {
    const [users, setUsers] = useState<LocalUser[]>([]);

    useEffect(() => {
        axios
            .get(`${BASE_URL}/Users`)
            .then((response: ApiResponse<LocalUser[]>) => {
                if (response.data) {
                    setUsers(response.data);
                }
            })
            .catch((error) => {
                console.error("Error fetching users:", error);
            });
    }, []);

    return (
        <div className="UsersPage">
            <Header as="h1">Users</Header>
            <Table celled>
                <Table.Header>
                    <Table.Row>
                        <Table.HeaderCell>ID</Table.HeaderCell>
                        <Table.HeaderCell>Email</Table.HeaderCell>
                        <Table.HeaderCell>First Name</Table.HeaderCell>
                        <Table.HeaderCell>Last Name</Table.HeaderCell>
                    </Table.Row>
                </Table.Header>

                <Table.Body>
                    {users.map((user) => (
                        <Table.Row key={user.id}>
                            <Table.Cell>{user.id}</Table.Cell>
                            <Table.Cell>{user.email}</Table.Cell>
                            <Table.Cell>{user.firstName}</Table.Cell>
                            <Table.Cell>{user.lastName}</Table.Cell>
                        </Table.Row>
                    ))}
                </Table.Body>
            </Table>
        </div>
    );
};

export default UsersPage;
