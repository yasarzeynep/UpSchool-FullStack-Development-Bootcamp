
import ProtectedRoute from "../components/ProtectedRoute.tsx";

const DashboardPage = () => {
    return (
        <ProtectedRoute>
            <div>
                {/* Sayfanın içeriği */}
                <h1>Dashboard Page</h1>
                <p>Welcome to Data Crawler!</p>
            </div>
        </ProtectedRoute>
    );
};

export default DashboardPage;