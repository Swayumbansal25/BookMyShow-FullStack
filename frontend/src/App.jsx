import { BrowserRouter as Router, Routes, Route, Navigate, useLocation } from 'react-router-dom';
import LoginPage from './pages/LoginPage';
import RegisterPage from './pages/RegisterPage';
import Home from './pages/Home';
import MovieDetails from './pages/MovieDetails';
import ShowtimeSelection from './pages/ShowtimeSelection'; 
import SeatSelection from './pages/SeatSelection';
import BookingSummary from './pages/BookingSummary'
import BookingSuccess from './pages/BookingSuccess';
import MyBookings from './pages/MyBookings';
import Navbar from './components/Navbar';

const PrivateRoute = ({ children }) => {
  const user = localStorage.getItem('user'); 
  return user ? children : <Navigate to="/login" />;
};

const AppContent = () => {
  const location = useLocation();
  const user = localStorage.getItem('user');
  
  // Navbar is ONLY shown if user is logged in AND not on login/register pages
  const hideNavbar = ['/login', '/register', '/'].includes(location.pathname) || !user;

  return (
    <>
      {!hideNavbar && <Navbar />} 
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route path="/register" element={<RegisterPage />} />
        
        <Route path="/home" element={<PrivateRoute><Home /></PrivateRoute>} />
        <Route path="/movie/:id" element={<PrivateRoute><MovieDetails /></PrivateRoute>} />
        
        {/* Match exactly with MovieDetails navigate call */}
        <Route path="/booking/:movieId" element={<PrivateRoute><ShowtimeSelection /></PrivateRoute>} />
        
        <Route path="/seats/:showId" element={<PrivateRoute><SeatSelection /></PrivateRoute>} />
        <Route path="/payment/:showId" element={<PrivateRoute><BookingSummary /></PrivateRoute>} />
        <Route path="/success" element={<PrivateRoute><BookingSuccess /></PrivateRoute>} />
        <Route path="/my-bookings" element={<PrivateRoute><MyBookings /></PrivateRoute>} />
        
        <Route path="/" element={<Navigate to="/login" />} />
        <Route path="*" element={<Navigate to="/login" />} />
      </Routes>
    </>
  );
};

function App() {
  return (
    <Router>
      <AppContent />
    </Router>
  );
}

export default App;