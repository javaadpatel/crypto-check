import React, { useState } from "react";
import { Link } from "react-router-dom";
import { Menu, MenuItemProps } from "semantic-ui-react";

function NavBar() {
  const [activeItem, setActiveItem] = useState("home");

  const handleItemClick = (
    e: React.MouseEvent<HTMLAnchorElement | MouseEvent>,
    data: MenuItemProps
  ) => setActiveItem(data.name as string);

  return (
    <div>
      <Menu pointing secondary>
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
