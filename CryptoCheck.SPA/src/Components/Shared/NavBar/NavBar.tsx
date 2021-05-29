import React, { useState, useEffect } from "react";
import { Link, useLocation } from "react-router-dom";
import {
  Menu,
  Button,
  MenuItemProps,
  Image,
  Dropdown,
  Header,
  DropdownItemProps,
  Icon,
} from "semantic-ui-react";
import "./navbar-styles.css";
// import logoSrc from "../../../images/logo.png";

function NavBar() {
  const [activeItem, setActiveItem] = useState("home");

  const handleItemClick = (
    e: React.MouseEvent<HTMLAnchorElement | MouseEvent>,
    data: MenuItemProps
  ) => setActiveItem(data.name as string);

  return (
    <div className="floodrunner-menu-container">
        <Menu className="floodrunner-menu" pointing secondary>
            <Menu.Item
              name="home"
              active={activeItem === "home"}
              onClick={handleItemClick}
              as={Link}
              to="/"
            >
              Home
            </Menu.Item>
        </Menu>
    </div>
  );
}

export default NavBar;
