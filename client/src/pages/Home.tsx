import { useState, useCallback, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import api from "../api";
import UrlForm from "../components/UrlForm";
import Dashboard from "./Dashboard";
import styles from "./Home.module.css";
import { FaLink } from "react-icons/fa";

interface HomeProps {
    showFormInitial?: boolean;
    editMode?: boolean;
}

export default function Home({ showFormInitial = false, editMode = false }: HomeProps) {
    const [shortUrl, setShortUrl] = useState<string | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState(false);
    const [showDashboard, setShowDashboard] = useState(!showFormInitial && !editMode);
    const [dashboardKey, setDashboardKey] = useState(0);
    const { id } = useParams();
    const navigate = useNavigate();

    useEffect(() => {
        // Se estiver em modo edição, pode buscar dados da URL para edição
        if (editMode && id) {
            setShowDashboard(false);
            // Aqui você pode buscar a URL pelo id e preencher o formulário se quiser
        }
    }, [editMode, id]);

    const handleShorten = async (inputUrl: string) => {
        setError(null);
        setShortUrl(null);
        setLoading(true);
        try {
            const response = await api.post("/ShortUrl", { originalUrl: inputUrl });
            setShortUrl(response.data.shortUrl);
            setShowDashboard(true);
            setDashboardKey(k => k + 1);
            navigate("/home");
        } catch {
            setError("An error occurred.");
        } finally {
            setLoading(false);
        }
    };

    const handleShowForm = useCallback(() => {
        setShowDashboard(false);
        setShortUrl(null);
        setError(null);
        navigate("/home/new");
    }, [navigate]);

    const handleShowDashboard = useCallback(() => {
        setShowDashboard(true);
        setShortUrl(null);
        setError(null);
        setDashboardKey(k => k + 1);
        navigate("/home");
    }, [navigate]);

    return (
        <div className={styles["home-container"]}>
            <div className={styles["header-area"]}>
                <h1 className={styles["home-title"]}>
                    <FaLink style={{ verticalAlign: "middle", marginRight: 8 }} />
                    URL Shortener
                </h1>
                <div className={styles["short-btn-area"]}>
                    {showDashboard ? (
                        <button
                            className={styles["toggle-btn"]}
                            onClick={handleShowForm}
                        >
                            Short URL
                        </button>
                    ) : (
                        <button
                            className={styles["toggle-btn"]}
                            onClick={handleShowDashboard}
                        >
                            Back
                        </button>
                    )}
                </div>
            </div>

            {showDashboard ? (
                <Dashboard key={dashboardKey} />
            ) : (
                <>
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
                </>
            )}
        </div>
    );
}