import { useEffect, useState } from "react";
import api from "../api";
import styles from "./Dashboard.module.css";
import { FaTrash, FaRegEdit } from "react-icons/fa";
import { Link } from "react-router-dom";

interface ShortUrl {
    id: string;
    originalUrl: string;
    shortUrl: string;
    createdAt: string;
}

export default function Dashboard() {
    const [urls, setUrls] = useState<ShortUrl[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState<string | null>(null);
    const [editingId, setEditingId] = useState<string | null>(null);
    const [editValue, setEditValue] = useState("");
    const [copiedId, setCopiedId] = useState<string | null>(null);

    useEffect(() => {
        api.get("/ShortUrl")
            .then(res => setUrls(res.data))
            .catch(() => setError("Failed to load URLs"))
            .finally(() => setLoading(false));
    }, []);

    const handleDelete = async (id: string) => {
        if (!window.confirm("Are you sure you want to delete this URL?")) return;
        try {
            await api.delete(`/ShortUrl/${id}`);
            setUrls(urls => urls.filter(u => u.id !== id));
        } catch {
            alert("Failed to delete URL.");
        }
    };


    const handleEditSave = async (url: ShortUrl) => {
        try {
            await api.put(`/ShortUrl/${url.id}`, {
                ...url,
                originalUrl: editValue
            });
            setUrls(urls =>
                urls.map(u =>
                    u.id === url.id ? { ...u, originalUrl: editValue } : u
                )
            );
            setEditingId(null);
        } catch {
            alert("Failed to update URL.");
        }
    };

    const handleEditCancel = () => {
        setEditingId(null);
    };

    const handleCopy = (shortUrl: string, id: string) => {
        navigator.clipboard.writeText(shortUrl);
        setCopiedId(id);
        setTimeout(() => setCopiedId(null), 1200);
    };

    if (loading) return <div>Loading...</div>;
    if (error) return <div className={styles["error-message"]}>{error}</div>;

    return (
        <div className={styles["dashboard-container"]}>
            <h2 className={styles["dashboard-title"]}>Your Shortened URLs</h2>
            <table className={styles["url-table"]}>
                <thead>
                    <tr>
                        <th>Original URL</th>
                        <th>Short URL</th>
                        <th>Created At</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {urls.map(url => (
                        <tr key={url.id}>
                            <td>
                                {editingId === url.id ? (
                                    <input
                                        value={editValue}
                                        onChange={e => setEditValue(e.target.value)}
                                        style={{ width: "100%" }}
                                    />
                                ) : (
                                    <a href={url.originalUrl} target="_blank" rel="noopener noreferrer">
                                        {url.originalUrl}
                                    </a>
                                )}
                            </td>
                            <td>
                                <div className={styles["short-url-cell"]}>
                                    <a href={url.shortUrl} target="_blank" rel="noopener noreferrer">
                                        {url.shortUrl}
                                    </a>
                                    <button
                                        className={styles["copy-btn"]}
                                        onClick={() => handleCopy(url.shortUrl, url.id)}
                                    >
                                        {copiedId === url.id ? "Copied!" : "Copy"}
                                    </button>
                                </div>
                            </td>
                            <td>{new Date(url.createdAt).toLocaleString()}</td>
                            <td>
                                {editingId === url.id ? (
                                    <>
                                        <button onClick={() => handleEditSave(url)}>Save</button>
                                        <button onClick={handleEditCancel} className={styles["delete-btn"]}>Cancel</button>
                                    </>
                                ) : (
                                    <>
                                        <Link to={`/home/edit/${url.id}`} className={styles["icon-btn"]} title="Edit">
                                            <FaRegEdit />
                                        </Link>
                                        <button
                                            title="Delete"
                                            onClick={() => handleDelete(url.id)}
                                            className={`${styles["icon-btn"]} ${styles["delete-btn"]}`}
                                        >
                                            <FaTrash />
                                        </button>
                                    </>
                                )}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>
        </div>
    );
}