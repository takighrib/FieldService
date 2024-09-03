import React, { useState, useEffect } from "react";
import 'bootstrap/dist/css/bootstrap.min.css';
import ServiceOrder from "../MyServiceOrders/ServiceOrder";
import { getServiceOrders } from "../../api/ServiceOrder";
import { getCompanies } from "../../api/company";
import ServiceOrderTable from "./ServiceOrderTable";

function Tabs() {
  const [activeTab, setActiveTab] = useState('tab1');
  const [selectedItem, setSelectedItem] = useState(null);
  const [serviceOrders, setServiceOrders] = useState([]);
  const [companies, setCompanies] = useState([]);

  const handleItemClick = (item) => {
    setSelectedItem(item);
    console.log(item);
    setActiveTab('itemDetails');
    console.log("the new one", item);

  };
  const handleFormClick = () => {

    setActiveTab('itemAdd');
    console.log("the new one itemAdd");

  };

  useEffect(() => {
    const fetchServiceOrders = async () => {
      try {
        const ordersData = await getServiceOrders();
        setServiceOrders(ordersData);
      } catch (error) {
        console.error("Error fetching service orders:", error);
        alert("Failed to fetch service orders");
      }
    };

    const fetchCompanies = async () => {
      try {
        const companiesData = await getCompanies();
        setCompanies(companiesData);
      } catch (error) {
        console.error("Error fetching companies:", error);
        alert("Failed to fetch companies");
      }
    };

    fetchServiceOrders();
    fetchCompanies();
  }, []);

  return (
    <div>
      <nav className="nav nav-tabs">
        <button
          className={`nav-link ${activeTab === 'tab1' ? 'active' : ''}`}
          onClick={() => setActiveTab('tab1')}
        >
          Tab 1
        </button>

        {activeTab === 'itemDetails' && (
          <button
            className={`nav-link ${activeTab === 'itemDetails' ? 'active' : ''}`}
            onClick={() => setActiveTab('itemDetails')}
          >
            Service order
            {selectedItem.id}
          </button>)}
        {activeTab === 'itemAdd' && (
          <button
            className={`nav-link ${activeTab === 'itemAdd' ? 'active' : ''}`}
            onClick={() => setActiveTab('itemAdd')}
          >
            My Form
          </button>

        )}
      </nav>
      <div className="tab-content mt-3">
        {activeTab === 'tab1' && <Content1 serviceOrders={serviceOrders} onItemClick={handleItemClick} handleFormClick={handleFormClick}/>}
        {activeTab === 'itemDetails' && selectedItem && <ServiceOrderTable order={selectedItem} />}
        {activeTab === 'itemAdd' && <Content2 />}

      </div>
    </div>
  );
}

function Content1({ serviceOrders, onItemClick , handleFormClick }) {
  return (
    <div>

      <button type="button" class="btn btn-primary"
        onClick={() => handleFormClick()}
      >Cliquer ici</button>

      <ul className="list-group">
        {serviceOrders.map((order) => (
          <li
            key={order.id}
            className="list-group-item d-flex justify-content-between align-items-center"
            onClick={() => onItemClick(order)}
            style={{ cursor: 'pointer' }}
          >
            Order ID: {order.id}
            <span className="badge bg-primary rounded-pill">{order.status}</span>
          </li>
        ))}
      </ul>
    </div>
  );
}
function Content2({ }) {
  return (
    <div>
      <h1>bonjour</h1>
    </div>
  );
}

export default Tabs;
