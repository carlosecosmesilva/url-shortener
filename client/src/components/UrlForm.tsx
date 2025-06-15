import React, { useState } from "react";

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
        <form className="home-form" onSubmit={handleSubmit}>
            <label htmlFor="url">Paste your long URL:</label>
            <input
                id="url"
                type="url"
                value={input}
                onChange={e => setInput(e.target.value)}
                placeholder="https://example.com"
                required
            />
            <button type="submit" disabled={loading}>
                {loading ? "Shortening..." : "Shorten"}
            </button>
        </form>
    );
}