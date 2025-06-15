import React, { useState } from "react";
import styles from "./UrlForm.module.css";

interface UrlFormProps {
    onSubmit: (url: string) => Promise<void>;
    loading?: boolean;
}

export default function UrlForm({ onSubmit, loading }: UrlFormProps) {
    const [input, setInput] = useState("");

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        await onSubmit(input);
        setInput("");
    };

    return (
        <form className={styles["url-form-container"]} onSubmit={handleSubmit}>
            <label htmlFor="url" className={styles["url-form-label"]}>
                Paste your long URL:
            </label>
            <div className={styles["url-form-row"]}>
                <input
                    id="url"
                    type="url"
                    value={input}
                    onChange={e => setInput(e.target.value)}
                    placeholder="https://example.com"
                    required
                    className={styles["url-form-input"]}
                />
                <button
                    type="submit"
                    disabled={loading}
                    className={styles["url-form-btn"]}
                >
                    {loading ? "Shortening..." : "Shorten"}
                </button>
            </div>
        </form>
    );
}