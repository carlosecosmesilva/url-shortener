import { useState } from "react";
import api from "../api";
import UrlForm from "../components/UrlForm";
import styles from "./Home.module.css";

export default function Home() {
    const [shortUrl, setShortUrl] = useState<string | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);

    const handleShorten = async (originalUrl: string) => {
        setError(null);
        setShortUrl(null);
        setLoading(true);
        try {
            const response = await api.post("/ShortUrl", { originalUrl });
            setShortUrl(response.data.shortUrl);
        } catch (err: unknown) {
            if (err && typeof err === "object" && "response" in err && err.response && typeof err.response === "object" && "data" in err.response && err.response.data && typeof err.response.data === "object" && "message" in err.response.data) {
                setError((err.response as { data: { message?: string } }).data.message || "An error occurred.");
            } else {
                setError("An error occurred.");
            }
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className={styles["home-container"]}>
            <h1 className="home-title">🔗 URL Shortener</h1>
            <UrlForm onSubmit={handleShorten} loading={loading} />

            {shortUrl && (
                <div className="short-url-box">
                    <strong>Short URL:</strong>
                    <a href={shortUrl} target="_blank" rel="noopener noreferrer">
                        {shortUrl}
                    </a>
                </div>
            )}

            {error && (
                <div className="error-message">
                    {error}
                </div>
            )}
        </div>
    );
}