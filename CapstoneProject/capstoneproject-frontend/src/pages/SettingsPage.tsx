import React, { useState } from "react";
import { toast } from "react-toastify";
import axios from "axios";
import { Radio, Button } from "semantic-ui-react";

const BASE_URL = "https://localhost:7245/api";

const SettingsPage = () => {
    const [selectedOption, setSelectedOption] = useState<string>("none");

    const handleOptionChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        setSelectedOption(event.target.value);
    };

    const handleSaveSettings = () => {
        axios
            .post(`${BASE_URL}/Settings`, { option: selectedOption })
            .then(() => {
                toast.success("Settings saved successfully!");
            })
            .catch((error) => {
                console.error("Error saving settings:", error);
                toast.error("Error saving settings!");
            });
    };

    return (
        <div className="SettingsPage">
            <h1>Notification Settings</h1>
            <h3>How would you like to receive notifications?</h3>
            <div className="radioGroup">
                <Radio
                    label="Email"
                    name="notificationOption"
                    value="email"
                    checked={selectedOption === "email"}
                    onChange={handleOptionChange}
                />
                <Radio
                    label="In-App Notification"
                    name="notificationOption"
                    value="inApp"
                    checked={selectedOption === "inApp"}
                    onChange={handleOptionChange}
                />
                <Radio
                    label="None"
                    name="notificationOption"
                    value="none"
                    checked={selectedOption === "none"}
                    onChange={handleOptionChange}
                />
            </div>
            <Button primary onClick={handleSaveSettings}>
                Save
            </Button>
        </div>
    );
};

export default SettingsPage;
