import React, { useState } from "react";
import api from "../api";
import { useNavigate } from "react-router-dom";
import styles from "./Login.module.css";

export default function Login() {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setError(null);
        try {
            const response = await api.post("/auth/login", { username, password });
            localStorage.setItem("accessToken", response.data.accessToken);
            localStorage.setItem("refreshToken", response.data.refreshToken);
            navigate("/");
        } catch (err: unknown) {
            type ErrorResponse = { response?: { data: string } };
            if (
                err &&
                typeof err === "object" &&
                "response" in err &&
                (err as ErrorResponse).response &&
                typeof (err as ErrorResponse).response?.data === "string"
            ) {
                setError((err as ErrorResponse).response!.data);
            } else {
                setError("Login failed");
            }
        }
    };

    return (
        <div className={styles["login-container"]}>
            <h1 className={styles["login-title"]}>Login</h1>
            <form className={styles["login-form"]} onSubmit={handleSubmit}>
                <label htmlFor="username">Username:</label>
                <input
                    id="username"
                    type="text"
                    value={username}
                    onChange={e => setUsername(e.target.value)}
                    required
                />
                <label htmlFor="password">Password:</label>
                <input
                    id="password"
                    type="password"
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                    required
                />
                <button type="submit">Login</button>
            </form>
            {error && <div className="error-message">{error}</div>}
        </div>
    );
}